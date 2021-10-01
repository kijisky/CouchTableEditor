tableEditor = angular.module('tableEditor', []);

tableEditor.service('logger', function () {
    return {
        log: function (msg) {
            console.log(msg);
        }
    }

})


tableEditor.factory('config', function () {
    return {
        baseUrl: "/api/"
    };
})

tableEditor.factory('svcDDL', function (config) {
    return {
        GetTables: function () {
            var t = fetch(config.baseUrl + "table");
            t = t.then(function (resp) {
                return resp.json();
            })
            return t;
        },
        GetTable: function (tblId) {
            var t = fetch(config.baseUrl + "table/" + tblId);
            t = t.then(function (resp) {
                return resp.json();
            })
            return t;
        },

        SaveField: function (tblId, fld) {
            var t = fetch(config.baseUrl + "table/" + tblId + "/field/" + fld.name, {
                method: "PUT",
                headers: {
                    'Content-type': 'application/json; charset=UTF-8' // Indicates the content 
                },
                body: JSON.stringify(fld)
            });
            return t;
        },
        DeleteField: function (tblId, fld) {
            var t = fetch(config.baseUrl + "table/" + tblId + "/field/" + fld.name, {
                method: "DELETE"
            });
            return t;

        },

        log: function (x) {
            console.log("message: " + x);
            console.log(config.baseUrl);
        }
    };
})





tableEditor.factory('svcDML', function (config) {
    return {
        GetTables: function () {
            var t = fetch(config.baseUrl + "table");
            t = t.then(function (resp) {
                return resp.json();
            })
            return t;
        },
        GetRows: function (tblId, fields) {
            var urlGetRows = config.baseUrl + "table/" + tblId + "/rows?";
            urlGetRows += fields.map(f => "&fields=" + f).join("");

            var t = fetch(urlGetRows);
            t = t.then(function (resp) {
                return resp.json();
            })
            return t;
        },

        SaveRow: function (tblId, rowId, rowData) {
            var t = fetch(config.baseUrl + "table/" + tblId + "/rows/" + rowId, {
                method: "PUT",
                headers: {
                    'Content-type': 'application/json; charset=UTF-8' // Indicates the content 
                },
                body: JSON.stringify(rowData)
            });
            return t;
        },
        DeleteRow: function (tblId, rowId, rowData) {
            var t = fetch(config.baseUrl + "table/" + tblId + "/rows/" + rowId, {
                method: "DELETE"
            });
            return t;

        },

        log: function (x) {
            console.log("message: " + x);
            console.log(config.baseUrl);
        }
    };
})