anychart.onDocumentReady(function () {
    anychart.data.loadCsvFile(
      'https://gist.githubusercontent.com/shacheeswadia/cd509e0b0c03964ca86ae7d894137043/raw/5f336c644ad61728dbac93026f3268b86b8d0680/teslaDailyData.csv',
      function (data) {
        
        // set chart theme
        anychart.theme('monochrome');
        
        // create data table on loaded data
        var dataTable = anychart.data.table();
        dataTable.addData(data);
  
        // map loaded data for the candlestick series
        var mapping = dataTable.mapAs({
          open: 1,
          high: 2,
          low: 3,
          close: 4
        });
  
        // create stock chart
        var chart = anychart.stock();
  
        // create first plot on the chart
        var plot = chart.plot(0);
        
        // set grid settings
        plot.yGrid(true).xGrid(true).yMinorGrid(true).xMinorGrid(true);
  
       
        var series = plot.candlestick(mapping);
        series.name('Tesla');
        series.legendItem().iconType('rising-falling');
        
         // create EMA indicators with period 50
        plot
          .ema(dataTable.mapAs({ value: 4 }))
          .series()
          .stroke('1.5 #455a64');
        
        // create envelope indicator
        chart.plot().env(mapping);
  
        
         // disable X axis for first plot
        plot.xAxis().enabled(false);
        
         // second plot to show macd values
        var macdPlot = chart.plot(1);
  
        var macdIndicator = macdPlot.macd(mapping);
        // set series type for histogram series.
        macdIndicator.histogramSeries('area');
  
        macdIndicator
          .histogramSeries()
          .normal()
          .fill('green .3')
          .stroke('green');
        macdIndicator
          .histogramSeries()
          .normal()
          .negativeFill('red .3')
          .negativeStroke('red');
      
        // set second plot's height
        macdPlot.height('40%');
        
        // create annotation
        var annotation = plot.annotations();
  
        // create annotation rectangle
        annotation.rectangle({
          // X - part of the first anchor
          xAnchor: '2021-11-08',
          // Y - part of the first anchor
          valueAnchor: 950,
          // X - part of the second anchor
          secondXAnchor: '2021-11-26',
          // Y - part of the second anchor
          secondValueAnchor: 1250,
          // set stroke settings
          stroke: '3 #c20000',
          // set fill settings
          fill: '#c20000 0.25'
        });
        
        // create annotation and set settings
        annotation
          .label()
          .xAnchor('2021-11-26')
          .valueAnchor(950)
          .anchor('right-top')
          .offsetY(5)
          .padding(6)
          .text('Elon Musk sells Tesla stock worth $1.05 billion')
          .fontColor('#fff')
          .background({
            fill: '#c20000 0.75',
            stroke: '0.5 #c20000',
            corners: 2
          });
  
  
        // create scroller series with mapped data
        chart.scroller().candlestick(mapping);
  
        // set chart selected date/time range
        chart.selectRange('2021-09-27', '2021-11-26');
  
        // create range picker
        var rangePicker = anychart.ui.rangePicker();
        
        // init range picker
        rangePicker.render(chart);
  
        // create range selector
        var rangeSelector = anychart.ui.rangeSelector();
        
        // init range selector
        rangeSelector.render(chart);
        
         // sets the title of the chart
        chart.title('Stock Chart');
        
        // set container id for the chart
        chart.container('graph');
        
        // initiate chart drawing
        chart.draw();
      }
    );
  });