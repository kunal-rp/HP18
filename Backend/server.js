var express = require('express');
var spawn = require("child_process").spawn;


//Set Up server
app = express();
var server = app.listen(3000);


app.get('/',function(req,res, next){

  res.send("UNDER construction ")
  
})
