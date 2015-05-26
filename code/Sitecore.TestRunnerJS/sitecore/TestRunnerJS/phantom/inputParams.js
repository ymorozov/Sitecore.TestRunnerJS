(function () {
  var extObj = {
    parse: function (args) {

    }
  }

  if (typeof module != 'undefined') module.exports = extObj;
  else define(function() { return extObj; });
})();
