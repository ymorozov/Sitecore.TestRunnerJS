(function () {
  define(['sinon'], function () {
    var server = sinon.fakeServer.create();
    server.xhr.useFilters = true;
    server.autoRespond = true;

    var filter = function (method, url, async, username, password) {
      // MATCH : /?sc_itemid={64D170BF-507C-4D53-BB4F-8FC76F5F2BBC}&sc_database=core
      if (method === "GET" && url.match("(?=.*sc_itemid={)(?=.*sc_database=core)")) {
        return true;
      }

      // MATCH : /sitecore/shell/client/Speak/Assets/lib/core/1.2/deps/require.js?_=1411242421633
      if (method === "GET" && url.match("require.js")) {
        return true;
      }

      return false;
    };

    server.xhr.addFilter(filter);

    return server;
  });
})();