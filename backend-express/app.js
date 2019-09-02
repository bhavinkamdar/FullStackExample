const express = require('express');
var bodyParser = require('body-parser');
const {
	exec
} = require('child_process');
var fs = require('fs');
const glob = require('glob');
var path = require('path');
var session = require('express-session')

const app = express()
app.use(bodyParser.json()); // for parsing application/json
app.use(bodyParser.urlencoded({
	extended: true
})); // for parsing application/x-www-form-urlencode
const port = 3000

app.use(session({
	secret: 'embedosEngineering#1'  ,
	resave: false,
	saveUninitialized: true,
}));

// app.use(express.static(__dirname + '/view'));

var controllers = glob.sync(__dirname + '/controllers/*.js');
controllers.forEach((controller) => {
	require(controller)(app);
});

app.use(function (req, res, next) {
	var err = new Error('File not found:' + req.url);
	err.status = 404;
	err.handled = 1;
	console.log('File not found:' + req.action + " " + req.url);
	next(err);
});

app.use(function (err, req, res, next) {
	if (!err.handled) {
		console.log(err);
	}
	res.status(err.status || 500);
	res.send(err.message);
});

app.listen(port, () => console.log(`App listening on port ${port}!`))