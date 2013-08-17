<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="JavascriptORM.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type = "text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
    <script type = "text/javascript" src="Scripts/handyOrm.min.js"></script>
    <link href = "Styles/Site.css" rel = "Stylesheet" type = "text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id = "container" style="margin: 0 auto; text-align:center;">
        <div id = "divAddButton" style = "width:180px;margin: 0 auto;">
            <div class = 'button'  onclick = "currentOperation = 'Insert';myModel.clearFields(['name', 'phone', 'address']);Refresh();">Add New Contact</div>
        </div>
        <div id="divContactList" ></div>
        <div id="divFields" style = "display:none;">
            <input id = "txtId" class = "textbox" maxlength = "5" placeholder = "Id" model-name = "MyModel" model-column = "id" disabled = "true"/> <br />
            <input id = "txtName" class = "textbox" maxlength = "20" placeholder = "Name" model-name = "MyModel" model-column = "name"/> <br />
            <input id = "txtPhone" class = "textbox" maxlength = "20" placeholder = "Phone Number" model-name = "MyModel" model-column = "phone"/> <br />
            <textarea id = "txtAddress" rows = "4" cols = "20" class = "textbox"  placeholder = "Address" model-name = "MyModel" model-column = "address"></textarea> <br />
            <div class = 'button'  style = "width:100px;" onclick = "SaveRecord();">Save Data</div>
            <div class = 'button' style = "width:100px;" onclick = "currentOperation = 'Read';Refresh();">Cancel</div>
        </div>
    </div>
    </form>

    <script type="text/javascript">
        var myModel = null;
        var currentOperation = "Read";

        //This will refresh the list of contacts being displayed on the page
        function RefreshContactList(r) {
            i = 0;
            contactListHtml = "";
            contactListHtml = "<table class='table-style' width='60%' style='margin: 0 auto;'><tr><th>Id</th><th>Name</th><th>Phone</th><th>Address</th><th></th><th></th></tr>"

            for (; i < r.length; i++) {
                contactListHtml += "<tr>";
                contactListHtml += "<td>" + r[i].id + "</td>";
                contactListHtml += "<td>" + r[i].name + "</td>";
                contactListHtml += "<td>" + r[i].phone + "</td>";
                contactListHtml += "<td>" + r[i].address + "</td>";
                contactListHtml += "<td><div class = 'button' onclick = \"EditContact('" + r[i].id + "', '" + r[i].name + "', '" + r[i].phone + "', '" + r[i].address + "');return true; \">Edit</div></td>";
                contactListHtml += "<td><div class = 'button' onclick = \"currentOperation = 'Delete';SaveRecord('" + r[i].id + "');return true;\" >Delete</div></td>";
                contactListHtml += "</tr>";
            }
            contactListHtml += "</table>";

            $("#divContactList").html(contactListHtml);            
        }

        //Edit the contact
        function EditContact(id, name, phone, address) {
            currentOperation = 'Update';
            $("#" + myModel.columnElementId("id")).val(id);
            $("#" + myModel.columnElementId("name")).val(name);
            $("#" + myModel.columnElementId("phone")).val(phone);
            $("#" + myModel.columnElementId("address")).val(address);
            Refresh();
        }

        //Refresh the page based on the current operation type
        function Refresh() {
            if (currentOperation === "Read") {
                $("#divContactList").css("display", "block");
                $("#divAddButton").css("display", "block");
                $("#divFields").css("display", "none");                           
            }
            else if (currentOperation === "Insert"
                    || currentOperation === "Update") {
                if (currentOperation === "Insert") {
                    $("#txtId").css("display", "none");
                }
                else {
                    $("#txtId").css("display", "inline");                                   
                }
                $("#divContactList").css("display", "none");
                $("#divAddButton").css("display", "none");
                $("#divFields").css("display", "block");
            }
        }
        
        //Fired when DOM content is loaded
        function ContentLoaded() {
            myModel = new ModelDef()
                        .modelName("MyModel")
                        .serviceName("DataService")
                        .methodName("ExecuteModelOperation")
                        .columns(["id", "name", "phone", "address"]);

            ReadRecords();
        }

        //Read records
        function ReadRecords() {
            var waitHtml = "<div style='margin: 0 auto;'>Loading...Please Wait!</div>";
            $("#divContactList").html(waitHtml);
            myModel.operation("Read", null, null).then(function (d) {
                myModel.results = d.d;
                RefreshContactList(myModel.results);
                currentOperation = "Read";
                Refresh();
            }, handleError);
        }

        //Save record based on the operation type
        function SaveRecord(id) {
            
            if (currentOperation === "Insert") {
                myModel.operation("Insert", ["name", "phone", "address"], null).then(ReadRecords, handleError);
            }
            else if (currentOperation === "Update") {
                myModel.operation("Update", ["id", "name", "phone", "address"], null).then(ReadRecords, handleError);
            }
            else if (currentOperation === "Delete") {
                myModel.operation("Delete", null, [{ col: "id", val: id}]).then(ReadRecords, handleError);
            }
        }

        //Handle error
        function handleError() { console.log("Unfortunately an error has occured out of nowhere.") };

        //content loaded event handler
        window.addEventListener("DOMContentLoaded", ContentLoaded, false); 
    </script>
</body>
</html>
