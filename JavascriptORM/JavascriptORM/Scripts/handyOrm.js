; var ModelDef = null;
var HandyHelper = null;

(function () {

    //Object containing helper methods
    var Helper = {
        //We need to escape and un-escape strings in many cases so as to avoid the corruption of the
        //json that we are passing. This can also be used in many places in the web pages, for ex: sometimes
        //we need to call functions from onclick attributes then escaping and un-escaping is better
        //than tweaking the string concatenations.
        //http://stackoverflow.com/questions/1219860/html-encoding-in-javascript-jquery
        htmlEscape : function(str){
            return String(str)
            .replace(/&/g, '&amp;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;');
        },
        htmlUnescape: function(value){
            return String(value)
            .replace(/&quot;/g, '"')
            .replace(/&#39;/g, "'")
            .replace(/&lt;/g, '<')
            .replace(/&gt;/g, '>')
            .replace(/&amp;/g, '&');
        }      
    };

    //This is the model definition that will be used to create objects
    //for different models. We have to provide model name, web service name,
    // web service method name, and columns as parameters. Note that this
    //definition only provides an abstracted way of asynchronous communication
    //with the server. So you are still relied on your server side code 
    //to implement proper functionality to use the data that this model 
    //passes from the UI.
    //I have used jQuery here mostly because it is already cross browser
    //tested so I don't have to worry about the cross browser compatibility
    //for most of the things.
    var modelDef = function () {
        this._modelName = null;
        this._serviceName = null;
        this._methodName = null;

        //this array can be made optional but I have included this
        // so as to make the model concrete at the time of its initialization
        this._columns = [];
        this._lastOperation = null;

        this.results = null;
        return this;
    };
    modelDef.prototype = {
        clearFields: function (fields) { //clear all the fields that are bound to the columns of this model
            var i = 0;
            var control = null;
            for (; i < fields.length; i++) {
                control = $("*[model-name = '" + this._modelName + "']*[model-column = '" + fields[i] + "']");
                $(control[0]).val('');
            }
            return this;
        },
        columnElementId: function (columnName) { //returns the id of the DOM element that is bound to this column
            var control = null;
            if (jQuery.inArray(columnName, this._columns) !== -1) {
                control = $("*[model-name = '" + this._modelName + "']*[model-column = '" + columnName + "']");
            }
            if (control === null || control === undefined) {
                return control;
            }
            else {
                return control[0].id;
            }
        },
        lastOperation: function () { //returns the last operation that was tried to be executed
            return this._lastOperation;
        },
        modelName: function (name) { //sets the model name
            this._modelName = name;
            return this;
        },
        operation: function (operationType, boundValues, customValues) { //operation that we want to execute
            var colParams = [];
            var i = 0;
            var attribControl = null;
            var params = null;
            var setResults = this.setResults;

            //set the last operation value
            this._lastOperation = operationType;

            //set to empty array if these params are null or undefined
            if (customValues === null || customValues === undefined)
                customValues = [];

            if (boundValues === null || boundValues === undefined)
                boundValues = [];

            //lets get the values of columns which are bound to the fields
            for (; i < boundValues.length; i++) {
                attribControl = $("*[model-name = '" + this._modelName + "']*[model-column = '" + boundValues[i] + "']");
                colParams.push({ ColName: boundValues[i], ColValue: Helper.htmlEscape($(attribControl[0]).val()) });
            }

            //get the custom values passed
            for (i = 0; i < customValues.length; i++) {
                colParams.push({ ColName: customValues[i].col, ColValue: Helper.htmlEscape(customValues[i].val) });
            }

            params = "{'operationType':'" + operationType + "','values':'" + JSON.stringify(colParams) + "'}";
            var myService = new Service(this._serviceName, this._methodName, params);
            return myService.callService();
        },
        serviceName: function (name) {
            this._serviceName = name;
            return this;
        },
        methodName: function (name) {
            this._methodName = name;
            return this;
        },
        columns: function (cols) {
            this._columns = cols;
            return this;
        }
    };

    //Implemented a home made promise object but not using it for now
    var Promise = function () {
        this.resolved = null;
        this.rejected = null;
    };
    Promise.prototype = {
        then: function (onResolved, onRejected) {
            this.resolved = onResolved;
            this.rejected = onRejected;
        }
    };

    var Service = function (serviceName, methodName, methodParams) {
        this._serviceName = serviceName;
        this._methodName = methodName;
        this._methodParams = methodParams;
        return this;
    };
    Service.prototype = {
        callService: function () {
            var promise = new Promise();
            return $.ajax({
                type: "POST",
                url: "/" + this._serviceName + ".asmx/" + this._methodName,
                data: this._methodParams,
                contentType: "application/json; charset=utf-8",
                dataType: "json"//,
                //            success: function (result) { promise.resolved(result.d); }, //still deciding to use jQuery promise or create my own
                //            error: function (e) { promise.rejected(e); }
            });
            //return promise;
        }
    };    

    //lets set our ModelDef and helper objects from this closed scope
    ModelDef = modelDef;
    HandyHelper = Helper;
})();