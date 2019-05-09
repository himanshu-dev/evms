
    $(document).ready(function () {
        //$("#failMessage").hide();
        $("#tab1, #tab2, #tab3").click(function () {
            $(this).addClass("active");
            $(this).siblings().removeClass("active");
        });
        $("#btnTake").click(function () {

            $("#res").attr({ "id": "res" });
            $("#form0").attr("action","/Voucher/SelectVoucher").submit();
        });
        function SearchFailed() {
            $("#failMessage").html("Voucher Selection Failed").show();
        }
        
    })
    var app = angular.module("myapp", []);
    app.controller("TodoController",function($scope){
        
    })