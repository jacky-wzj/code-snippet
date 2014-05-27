var iconv = require('iconv-lite');

var s=  'p';
var arr = new Buffer(s, 'utf8');
console.log(arr.toString('hex'));
console.log(arr.length);
console.log(arr[0]);
// console.log(arr[0].toString('hex'));