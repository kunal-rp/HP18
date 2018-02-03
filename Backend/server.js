

var express = require('express');
var spawn = require("child_process").spawn;


//Set Up server
app = express();
var server = app.listen(3000);


app.get('/',function(req,res, next){

  var process = spawn('python',["python/test.py", "New File"]);
  process.stdout.on('data', function (data){
    res.send(data.toString())
  });
})
