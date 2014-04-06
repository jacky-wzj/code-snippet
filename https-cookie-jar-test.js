var cookie = require('cookie-jar');
var https = require('https');

var options = {
  hostname: 'github.com',
  port: 443,
  path: '/',
  method: 'GET'
};

var req = https.request(options, function(res) {
  console.log("statusCode: ", res.statusCode);
  var cookies = res.headers['set-cookie'];
  var cookieStr = '';
  for(var i in cookies){
  	// console.log(cookies[i]);
  	var c = new cookie(cookies[i]);
  	cookieStr += c.name + "=" + c.value + ";"; 
  }
  console.log(cookieStr);

  res.on('data', function(d) {
  //   process.stdout.write(d);
  });
});
req.end();

req.on('error', function(e) {
  console.error(e);
});