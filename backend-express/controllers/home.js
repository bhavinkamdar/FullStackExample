const path = require('path');
const express = require('express');
const router = express.Router();

module.exports = (app) => {
  app.use('/', router);
};

router.get('/', (req, res, next) => {
  //return res.redirect('/editor');
  // if(req.session.user) return res.redirect('/realtime');
  return res.redirect('/login');
});
