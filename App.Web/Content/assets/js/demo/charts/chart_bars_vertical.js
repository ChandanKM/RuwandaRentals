/*
 * charts/chart_bars_vertical.js
 *
 * Demo JavaScript used on charts-page for "Vertical Bars".
 */

"use strict";

$(document).ready(function () {

    // Sample Data
    var d1 = [];
    var d2 = [];
    var d3 = [];
    var d4 = [];
    $.ajax({
        type: "POST",
        url: "/Vendor/GetLoginVendorId",
        //   dataType: "json",
        async: false,
        success: function (responses) {
            if (responses != "ss") {
    $.ajax({
        type: "GET",
        url: "/api/vendors/DashBoard",
        data: { vndr_Id: responses, Prop_Id: "", Days: 90 },
        dataType: "json",
        async: false,
        success: function (response) {
            


            for (var j = 0; j < response.Table4.length ; j++) {
                
                // month[j] = response.Table1[j].orders
               
                d1.push([j + 1, parseInt(response.Table4[j].sold)]);
                d2.push([j + 1, parseInt(response.Table4[j].hold)]);
                d4.push([j + 1, parseInt(response.Table4[j].total)]);
                d3.push([j + 1, parseInt(response.Table4[j].vendorprovided)]);

            }



        },
        error: function (jqxhr) {

            // Failed(JSON.parse(jqxhr.responseText));
        }
    });
            }
            else {
                alert('Oops You might not hav permission to view this page!!')
                // window.location.href='/Vendor/Bind'

            }
        },
        error: function (jqxhr) {

           // Failed(JSON.parse(jqxhr.responseText));
        }
    });
    //for (var i = 0; i <= 12; i += 1)
    //    d1.push([i, parseInt(Math.random() * 30)]);

    //var d2 = [];
    //for (var i = 0; i <= 12; i += 1)
    //    d2.push([i, parseInt(Math.random() * 30)]);

    //var d3 = [];
    //for (var i = 0; i <= 7; i += 1)
    //    d3.push([i, parseInt(Math.random() * 30)]);

    var ds = new Array();

    ds.push({
        label: "Sold",
        data: d1,
        bars: {
            show: true,
            barWidth: 0.2,
            order: 1
        }
    });
    ds.push({
        label: "Hold",
        data: d2,
        bars: {
            show: true,
            barWidth: 0.2,
            order: 2
        }
    });
    ds.push({
        label: "Room Inventory",
        data: d3,
        bars: {
            show: true,
            barWidth: 0.2,
            order: 3
        }
    });
    ds.push({
        label: "Total",
        data: d4,
        bars: {
            show: true,
            barWidth: 0.2,
            order: 3
        }
    });
    // Initialize Chart
    $.plot("#chart_bars_vertical", ds, $.extend(true, {}, Plugins.getFlotDefaults(), {
        series: {
            lines: { show: false },
            points: { show: false }
        },
        grid: {
            hoverable: true
        },
        tooltip: true,
        tooltipOpts: {
            content: '%s: %y'
        }
    }));

});