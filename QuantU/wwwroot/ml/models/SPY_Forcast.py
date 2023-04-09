import yfinance as yf
import pandas as pd
from pandas_datareader import data as pdr
import numpy as np
import matplotlib.pyplot as plt
from datetime import date
from datetime import timedelta
from sklearn.preprocessing import MinMaxScaler
from keras.models import Sequential
from keras.layers import Dense, LSTM
import csv
import schedule
import time


def main():
    begin = '1993-02-01'
    yf.pdr_override() 
    ticker = 'SPY'
    df = pdr.get_data_yahoo(ticker, begin)
    df.to_csv('/Users/antoinesfeir/Documents/GitHub/SWMG/QuantU/wwwroot/ml/SPY_StockPrices.csv')

    # Getting the dates and 30 days in the future
    dates = df.index

    new_dates = pd.date_range(dates[-1], periods=30, freq='D')
    dates = dates.append(new_dates)

    close = df['Close']
    ds = close.values

    #Using MinMaxScaler for normalizing data between 0 & 1
    normalizer = MinMaxScaler(feature_range=(0,1))
    ds_scaled = normalizer.fit_transform(np.array(ds).reshape(-1,1))

    #Defining test and train data sizes
    train_size = int(len(ds_scaled)*0.70) # train set is 70%
    test_size = len(ds_scaled) - train_size # test is the other 30%

    #Splitting data between train and test
    ds_train, ds_test = ds_scaled[0:train_size,:], ds_scaled[train_size:len(ds_scaled),: 1]

    #creating dataset in time series for LSTM model 
    def create_ds(dataset,step):
        Xtrain, Ytrain = [], []
        for i in range(len(dataset)-step-1):
            a = dataset[i:(i+step), 0]
            Xtrain.append(a)
            Ytrain.append(dataset[i + step, 0])
        return np.array(Xtrain), np.array(Ytrain)

    #Taking 100 days price as one record for training
    time_stamp = 100
    X_train, y_train = create_ds(ds_train,time_stamp)
    X_test, y_test = create_ds(ds_test,time_stamp)

    #Reshaping data to fit into LSTM model
    X_train = X_train.reshape(X_train.shape[0],X_train.shape[1] , 1)
    X_test = X_test.reshape(X_test.shape[0],X_test.shape[1] , 1)

    # Creating LSTM model
    model = Sequential()
    model.add(LSTM(100, return_sequences=True, input_shape=(X_train.shape[1], 1)))
    model.add(LSTM(100, return_sequences=False))
    model.add(Dense(25))
    model.add(Dense(1))

    #Training model with adam optimizer and mean squared error loss function
    model.compile(loss='mean_squared_error',optimizer='adam')
    model.fit(X_train,y_train,validation_data=(X_test,y_test),epochs=100,batch_size=64)

    #Predicitng on train and test data
    train_predict = model.predict(X_train)
    test_predict = model.predict(X_test)

    #Inverse transform to get actual value
    train_predict = normalizer.inverse_transform(train_predict)
    test_predict = normalizer.inverse_transform(test_predict)

    #Calculating RMSE for train and test data
    test = np.vstack((train_predict,test_predict))


    #Getting the last 100 days records
    last100days = len(ds_test) - 100
    fut_inp = ds_test[last100days:]

    fut_inp = fut_inp.reshape(1,-1)
    tmp_inp = list(fut_inp)

    #Creating list of the last 728 data
    tmp_inp = tmp_inp[0].tolist()

    #Predicting next 30 days price suing the current data
    #It will predict in sliding window manner (algorithm) with stride 1
    lst_output=[]
    n_steps=100
    i=0
    while(i<30):
        if(len(tmp_inp)>100):
            fut_inp = np.array(tmp_inp[1:])
            fut_inp=fut_inp.reshape(1,-1)
            fut_inp = fut_inp.reshape((1, n_steps, 1))
            yhat = model.predict(fut_inp, verbose=0)
            tmp_inp.extend(yhat[0].tolist())
            tmp_inp = tmp_inp[1:]
            lst_output.extend(yhat.tolist())
            i=i+1
        else:
            fut_inp = fut_inp.reshape((1, n_steps,1))
            yhat = model.predict(fut_inp, verbose=0)
            tmp_inp.extend(yhat[0].tolist())
            lst_output.extend(yhat.tolist())
            i=i+1


    temp = len(ds_scaled) - 100

    ds_new = ds_scaled.tolist()
    #Entends helps us to fill the missing value with approx value
    ds_new.extend(lst_output)

    #Creating final data for plotting
    final_graph = normalizer.inverse_transform(ds_new).tolist()

    #Plotting final results with predicted value after 30 Days
    plt.plot(final_graph,)
    plt.ylabel("Price")
    plt.xlabel("Time")
    plt.title("{0} prediction of next month's closing price".format(ticker))
    plt.axhline(y=final_graph[len(final_graph)-1], color = 'red', linestyle = ':', label = 'NEXT 30D: {0}'.format(round(float(*final_graph[len(final_graph)-1]),2)))
    plt.legend()
    plt.show()

    # Saving results to CSV file
    dates_list = dates.to_list()

    #   Converting date to string
    dates_str = [date.strftime('%Y-%m-%d') for date in dates_list]
    # Convert final_graph to a list of lists
    final_graph_list = [[price] for price in final_graph]

    # Modify the list comprehension to return the price value instead of a list containing the price value
    final_graph_list = [price for sublist in final_graph for price in sublist]

    # Concatenate dates_str and final_graph_list using zip
    data = [["Date", "Price"]] + list(zip(dates_str, final_graph_list))

    with open('/Users/antoinesfeir/Documents/GitHub/SWMG/QuantU/wwwroot/ml/SPY_Forcast.csv', 'w', newline='') as file:
       writer = csv.writer(file)
       writer.writerows(data)

#  Scheduling the script to run every day at 2:00 AM
schedule.every().day.at("2:00").do(main)

while True:
    schedule.run_pending()
    time.sleep(1)