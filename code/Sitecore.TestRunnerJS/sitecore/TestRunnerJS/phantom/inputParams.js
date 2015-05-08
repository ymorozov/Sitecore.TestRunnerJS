(function () {
  var extObj = {
    parse: function (args) {
      var settings = {
        instanceName: args[1],
        applicationName: args[2]
    }

      return settings;
    }
  }

  if (typeof module != 'undefined') module.exports = extObj;
  else define(function() { return extObj; });
})();
