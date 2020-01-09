/*
 * charts/chart_filled_red.js
 *
 * Demo JavaScript used on charts-page for "Filled Chart (Red)".
 */

"use strict";

$(document).ready(function(){


	// Sample Data
    var month = ["1","2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"];
    var d1;
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
                        


                        for (var j = 0; j < response.Table3.length ; j++) {

                            month[j] = response.Table3[j].orders

                        }


                        d1 = [[1262304000000, month[0]], [1264982400000, month[1]], [1267401600000, month[2]], [1270080000000, month[3]], [1272672000000, month[4]], [1275350400000, month[5]],
                      [1277942400000, month[6]], [1280620800000, month[7]], [1283299200000, month[8]], [1285891200000, month[9]], [1288569600000, month[10]], [1291161600000, month[11]]];
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

            Failed(JSON.parse(jqxhr.responseText));
        }
    });

	var data1 = [
		{ label: "Total Aborted", data: d1, color: App.getLayoutColorCode('red') }
	];

	$.plot("#chart_filled_red", data1, $.extend(true, {}, Plugins.getFlotDefaults(), {
		xaxis: {
			min: (new Date(2009, 12, 1)).getTime(),
			max: (new Date(2010, 11, 2)).getTime(),
			mode: "time",
			tickSize: [1, "month"],
			monthNames: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
			tickLength: 0
		},
		series: {
			lines: {
				fill: true,
				lineWidth: 1.5
			},
			points: {
				show: true,
				radius: 2.5,
				lineWidth: 1.1
			}
		},
		grid: {
			hoverable: true,
			clickable: true
		},
		tooltip: true,
		tooltipOpts: {
			content: '%s: %y'
		}
	}));


});