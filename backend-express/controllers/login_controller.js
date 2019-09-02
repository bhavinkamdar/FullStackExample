const path = require('path');
const express = require('express');
const router = express.Router();
const fs = require('fs');
const exec = require('child_process').exec;

var config_path = '/root';

module.exports = (app) => {
  app.use('/login', router);
};

// GET request handler, returns list of all usernames
router.get('/', (req, res) => {
    var usernames = ['admin', 'user1', 'user2', 'user3'];
    return res.send(usernames);
});

// POST request handler, returns whether the given username and password are valid
router.post('/', (req, res) => {
    console.log(req.body);
    // TODO: Put code to read username and password from database
    if(req.body.username == "admin" && req.body.password == "admin@123"){
        req.session.user = "admin"
        return res.status(200).send({status: "Valid login"});
    }  

    if(req.body.username == "user1" && req.body.password == "user1"){
        req.session.user = "user1"
        return res.status(200).send({status: "Valid login"});
    }  
    try{
        if(req.session){
            req.session.destroy();
        }
    } catch(err){

    }
    return res.status(404).send({status:"Invalid Username or Password"});
})
