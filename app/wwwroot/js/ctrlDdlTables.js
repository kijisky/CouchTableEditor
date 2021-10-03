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
            fldDesc.children.push({ IsNew: true });
        }

        ddl.addRootField = function () {
            if (!ddl.selectedTable) return;
            ddl.selectedTable.fields.push({ IsNew: true });
        }

        ddl.SaveField = function (fld) {
            if (!ddl.selectedTable) return;
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
                var indx = ddl.selectedTable.fields.indexOf(fld);
                ddl.selectedTable.fields.splice(indx, 1);
                $scope.$apply();
            });
        }

        ddl.OnEditField = function () {
            if (ddl.selectedField != null) {
                ddl.selectedField.IsDirty = true;
            }
        }

        ddl.LoadTablesList();
        ddl.LoadVocList();
    })