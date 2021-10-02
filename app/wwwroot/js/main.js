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
        GetRow: function (tblId, rowId) {
            var urlGetRows = config.baseUrl + "table/" + tblId + "/rows/" + rowId;

            var t = fetch(urlGetRows);
            t = t.then(function (resp) {
                return resp.json();
            })
            return t;
        },

        SaveRow: function (tblId, rowId, rowData) {
            var url = rowId ? config.baseUrl + "table/" + tblId + "/rows/" + rowId :
                config.baseUrl + "table/" + tblId + "/rows/";
            var t = fetch(url, {
                method: "PUT",
                headers: {
                    'Content-type': 'application/json; charset=UTF-8' // Indicates the content 
                },
                body: JSON.stringify(rowData)
            });
            return t;
        },
        DeleteRow: function (tblId, rowId) {
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






tableEditor.factory('svcVoc', function (config) {
    return {
        GetVocList: function () {
            var t = fetch(config.baseUrl + "vocabulary/");
            t = t.then(function (resp) {
                return resp.json();
            })
            return t;
        },
        GetVoc: function (vocId) {
            var urlGetRows = config.baseUrl + "vocabulary/" + vocId;

            var t = fetch(urlGetRows);
            t = t.then(function (resp) {
                return resp.json();
            })
            return t;
        },

        SaveVoc: function (vocId, vocData) {
            var url = config.baseUrl + "vocabulary/" + vocId;
            var t = fetch(url, {
                method: "PUT",
                headers: {
                    'Content-type': 'application/json; charset=UTF-8' // Indicates the content 
                },
                body: JSON.stringify(vocData)
            });
            return t;
        },

        DeleteVoc: function (vocId, vocData) {
            var url = config.baseUrl + "vocabulary/" + vocId;
            var t = fetch(url, {
                method: "DELETE"
            });
            return t;
        },

        GetVocTerms: function (vocId) {
            var urlGetRows = config.baseUrl + "vocabulary/" + vocId + "/terms/";

            var t = fetch(urlGetRows);
            t = t.then(function (resp) {
                return resp.json();
            })
            return t;
        },

        SaveTerm: function (vocId, termId, termData) {
            var url = config.baseUrl + "vocabulary/" + vocId + "/term/" + termId;
            var t = fetch(url, {
                method: "PUT",
                headers: {
                    'Content-type': 'application/json; charset=UTF-8' // Indicates the content 
                },
                body: JSON.stringify(termData)
            });
            return t;
        },

        DeleteTerm: function (vocId, termId) {
            var url = config.baseUrl + "vocabulary/" + vocId + "/term/" + termId;
            var t = fetch(url, {
                method: "DELETE"

            });
            return t;
        },

        LoadExternalVocabulary: function (extUrl, extPath, pTermId, pTermName) {
            var t = fetch(extUrl);
            t = t.then(function (resp) {
                return resp.json();
            }).then(function (respJson) {
                var termsList = respJson;
                if (extPath) {
                    termsList = eval("termsList." + extPath);
                }
                var ans = termsList.map(function (extTerm) {
                    newTerm = {
                        "id": eval("extTerm." + pTermId),
                        "name": eval("extTerm." + pTermName)
                    };
                    return newTerm;
                });
                ans.sort((a, b) => a.name > b.name && 1 || -1)
                return ans;
            })
            return t;
        },

        log: function (x) {
            console.log("message: " + x);
            console.log(config.baseUrl);
        }
    };
})