tableEditor
    .controller('dml', function (logger, svcDML, $scope) {
        dml = this;
        dml.rowsList = [];
        dml.tableCode = null;
        dml.show = {};

        dml.click = function () {
            console.log(this.tablesList);
        }

        dml.InitTableCode = function (tableCode) {
            dml.tableCode = tableCode;
            dml.LoadRows();
        }

        dml.GetVisibleFields = function () {
            var ans = [];
            for (var fieldName in dml.show) {
                if (dml.show[fieldName]) {
                    ans.push(fieldName);
                }
            }
            return ans;
        }

        dml.LoadRows = function () {
            var fieldsList = dml.GetVisibleFields();
            svcDML.GetRows(dml.tableCode, fieldsList).then(function (data) {
                dml.rowsList = data;
                logger.log("tables loaded");
                $scope.$apply();
            })
        }

        dml.AddRow = function () {
            dml.rowsList.push({ IsNew: true });
        }

        dml.SaveRow = function (row) {
            svcDML.SaveRow(dml.tableCode, row._id, row).then(function (data) {
                logger.log("Row saved");
                row.dirty = false;
                row.IsNew = false;
                $scope.$apply();
            })
        }

        dml.LoadRows();
    })