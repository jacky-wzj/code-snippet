var iconv = require('iconv-lite');

var s=  '你';
var arr = new Buffer(s, 'utf8');
console.log(arr.toString('hex'));