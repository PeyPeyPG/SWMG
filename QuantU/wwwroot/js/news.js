var requestOptions = {
    method: 'GET'
};

var params = {
    api_token: 'zN4Le7hdAZLXx50HoiqfMv00RgKQzX6i7RPsycKs',
    symbols: 'msft,fb',
    limit: '50'
};

var esc = encodeURIComponent;
var query = Object.keys(params)
    .map(function(k) {return esc(k) + '=' + esc(params[k]);})
    .join('&');

fetch("https://api.marketaux.com/v1/news/all?" + query, requestOptions)
  .then(response => response.json())
  .then(result => {
                  console.log(result);
                  document.getElementById('first').innerHTML = result.data[0].title;
                  document.getElementById('second').innerHTML = result.data[1].title;
                  document.getElementById('third').innerHTML = result.data[2].title;})
  .catch(error => console.log('error', error));