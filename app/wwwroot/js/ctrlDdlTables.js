tableEditor
    .controller('ddl', function (logger, svcDDL, svcVoc, $scope) {
        ddl = this;
        ddl.tablesList = [];
        ddl.vocList = [];
        ddl.selectedTable = null;
        ddl.selectedField = null;

        ddl.click = function () {
            logger.log("click!");
            ddl.LoadTablesList();
        }

        ddl.LoadTablesList = function () {
            svcDDL.GetTables().then(function (d) {
                ddl.tablesList = d;
                logger.log("tables loaded");
                console.log(ddl.tablesList);
                $scope.$apply();
            })
        }

        ddl.SelectField = function (fld) {
            ddl.selectedField = fld;
        }

        ddl.LoadVocList = function () {
            svcVoc.GetVocList().then(function (ajaxData) {
                ddl.vocList = ajaxData;
                $scope.$apply();
            });
        }

        ddl.LoadTable = function (pTable) {
            ddl.selectedTable = null;
            logger.log("load table: " + pTable.id);
            svcDDL.GetTable(pTable.id).then(function (loadedTbl) {
                ddl.selectedTable = loadedTbl;
                logger.log("loaded table: ");
                logger.log(loadedTbl);
                $scope.$apply();
            })
        }

        ddl.addChildFieldTo = function (fldDesc) {
            if (!fldDesc.children) {
                fldDesc.children = [];
            }

            var newField = { IsNew: true, parentPath: fldDesc.path };
            fldDesc.children.push(newField);
        }

        ddl.addRootField = function () {
            if (!ddl.selectedTable) return;
            var newField = { IsNew: true, parentPath: null };
            ddl.selectedTable.fields.push(newField);
        }

        ddl.SaveField = function (fld) {
            if (!ddl.selectedTable) return;
            if (fld.IsNew) {
                if (fld.parentPath) {
                    fld.path = fld.parentPath + ".";
                } else {
                    fld.path = "";
                }
                fld.path += fld.name;
            }
            svcDDL.SaveField(ddl.selectedTable.id, fld).then(function () {
                fld.IsNew = false;
                fld.IsDirty = false;
                logger.log("field saved");
                $scope.$apply();
            });
        }

        ddl.DeleteField = function (fld) {
            if (!ddl.selectedTable) return;
            var AreYouSure = confirm('Удалить поле?' + fld.name);
            if (!AreYouSure) {
                return;
            }

            svcDDL.DeleteField(ddl.selectedTable.id, fld).then(function () {
                logger.log("field deleted");
                fldParent = fld.IsNew ?
                    ddl.FindFieldByPath(fld.parentPath, 0)
                    : ddl.FindFieldByPath(fld.path, -1);

                parentFieldsList = fldParent ? fldParent.children : ddl.selectedTable.fields;

                var indx = parentFieldsList.indexOf(fld);
                if (indx > -1) {
                    parentFieldsList.splice(indx, 1);
                }
                $scope.$apply();
            });
        }

        ddl.OnEditField = function () {
            if (ddl.selectedField != null) {
                ddl.selectedField.IsDirty = true;
            }
        }

        ddl.FindFieldByPath = function(codeField, pDepth = 0)
        {
            var pathParts = codeField.split(".");

            if (pDepth == 0) pDepth = pathParts.length;
            if (pDepth < 0) pDepth = pathParts.length + pDepth;
            if (pDepth > pathParts.Length) pDepth = pathParts.length;

            var currLevelFieldsList = ddl.selectedTable.fields;
            levelField = null;
            for (var i = 0; i < pDepth; i++) {
                var curName = pathParts[i];
                levelField = null;
                for (var fld of currLevelFieldsList) {
                    if (fld.name == curName) {
                        levelField = fld;
                        continue;
                    }
                }
                if (levelField == null) {
                    return null;
                }
                currLevelFieldsList = levelField.children;
            }
            return levelField;
        }

        ddl.LoadTablesList();
        ddl.LoadVocList();
    })