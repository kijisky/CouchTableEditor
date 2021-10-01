tableEditor
    .controller('ddl', function (logger, svcDDL, $scope) {
        ddl = this;
        ddl.tablesList = [];
        ddl.selectedTable = null;

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

        ddl.addField = function () {
            if (!this.selectedTable) return;
            this.selectedTable.fields.push({ IsNew: true });
        }

        ddl.SaveField = function (fld) {
            if (!this.selectedTable) return;
            svcDDL.SaveField(this.selectedTable.id, fld).then(function () {
                fld.IsNew = false;
                fld.dirty = false;
                logger.log("field saved");
                $scope.$apply();
            });
        }

        ddl.DeleteField = function (fld) {
            if (!this.selectedTable) return;

            svcDDL.DeleteField(this.selectedTable.id, fld).then(function () {
                logger.log("field deleted");
                var indx = this.selectedTable.fields.indexOf(fld);
                this.selectedTable.fields.splice(indx, 1);
                $scope.$apply();
            });
        }

        ddl.LoadTablesList();
    })