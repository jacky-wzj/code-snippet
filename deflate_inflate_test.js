var arr = Array.prototype.map.call('Hello world', function (char) {
    return char.charCodeAt(0);
});
var arr = new Buffer('Hello World,ddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd', 'utf8');
var compressed = deflate.deflate(arr);
console.log(compressed);
var decompressed = deflate.inflate(compressed);
var buf = new Buffer(decompressed, 'utf8');
var str = iconv.decode(buf,'utf8');
console.log(str);