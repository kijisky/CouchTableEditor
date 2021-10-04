tableEditor
    .controller('dml', function (logger, svcDML, svcVoc, $scope) {
        dml = this;
        dml.rowsList = [];
        dml.tableCode = null;
        dml.show = {};
        dml.searchString = "";
        dml.ExtVoc = {};
        dml.selectedRow = {};
        dml.loadedFields = {};

        dml.click = function () {
            console.log(dml.tablesList);
        }

        dml.InitTableCode = function (tableCode) {
            dml.tableCode = tableCode;
            dml.LoadRows();
            dml.LoadVocabularies();
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

        dml.LoadVocabularies = function () {
            svcVoc.GetVocList().then(function (ajaxData) {
                dml.vocList = {}
                for (var vodIndex in ajaxData) {
                    var vocId = ajaxData[vodIndex].id;
                    var vocTerms = ajaxData[vodIndex].termsList;
                    dml.vocList[vocId] = vocTerms;
                }
                $scope.$apply();
            });
        }

        dml.onFieldsChange = function (fieldPath) {
            if (dml.loadedFields[fieldPath] || !dml.show[fieldPath]) {
                return;
            } else {
                dml.loadedFields[fieldPath] = true;
                dml.LoadRows();
            }
        }

        dml.MarkRowDirty = function (row) {
            if (row.IsDirty) return;

            row.IsDirty = true;
            for (var dataRow in dml.rowsList) {
                if (dml.rowsList[dataRow].data == row.data) {
                    dml.rowsList[dataRow].IsDirty = true;
                }
            }
        }


        dml.SelectRowReset = function () {
            dml.selectedRow.data = null;
            dml.selectedRow._id = null;
            dml.selectedRow.IsDirty = false;
        }
        dml.SelectRow = function (row) {
            dml.SelectRowReset();
            dml.selectedRow._id = row._id;

            svcDML.GetRow(dml.tableCode, row._id).then(function (rowObject) {
                row.data = rowObject.data;
                dml.selectedRow.data = rowObject.data;
                logger.log("loaded Row data");
                $scope.$apply();
            });

        }

        dml.LoadRows = function () {
            dml.SelectRowReset();
            var fieldsList = dml.GetVisibleFields();
            for (var flds in fieldsList) {
                this.loadedFields = {};
                this.loadedFields[flds] = true;
            }

            svcDML.GetRows(dml.tableCode, fieldsList).then(function (data) {
                dml.rowsList = data;
                logger.log("tables loaded");
                $scope.$apply();
            })
        }

        dml.StartEditVoc = function (row, fieldName, dictID) {
            if (!row.IsEditing) {
                row.IsEditing = {};
            }
            row.IsEditing[fieldName] = true;
        }

        dml.StartEditExtVoc = function (row, fieldName, extUrl, extPath, termId, termName) {
            if (!row.IsEditing) {
                row.IsEditing = {};
            }
            row.IsEditing[fieldName] = true;

            if (!dml.ExtVoc[fieldName]) {
                svcVoc.LoadExternalVocabulary(extUrl, extPath, termId, termName).then(function (termsList) {
                    dml.ExtVoc[fieldName] = termsList;
                    logger.log("ext voc loaded");
                    $scope.$apply();
                })
            }
        }



        dml.AddRow = function () {
            var newRow = { IsNew: true }
            dml.SelectRow(newRow);
            dml.rowsList.push(newRow);
        }

        dml.SaveRow = function (row) {
            svcDML.SaveRow(dml.tableCode, row._id, row).then(function (data) {
                logger.log("Row saved");
                row.IsDirty = false;
                row.IsNew = false;
                $scope.$apply();
            })
        }



        ////. SubTables

        dml.AddSubTableRow = function (rowData, subTablePath) {
            var subTable = eval("rowData." + subTablePath);
            var isFieldArray = subTable && Array.isArray(subTable);
            if (!isFieldArray) {
                eval("rowData." + subTablePath + " = [];");
            }
            var subTable = eval("rowData." + subTablePath);

            var newRow = { IsNew: true, IsEdititnd: true }
            subTable.push(newRow);
        }

        dml.LoadRows();
    })