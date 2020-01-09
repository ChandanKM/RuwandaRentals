/*
 * charts/chart_pie.js
 *
 * Demo JavaScript used on charts-page for "Pie Chart".
 */

"use strict";

$(document).ready(function(){


	// Sample Data
	var d_pie = [];
	var series = 3;
	//for (var i = 0; i<series; i++) {
	//	d_pie[i] = { label: "Series "+(i+1), data: 33 }
	//}
	$.ajax({
	    type: "POST",
	    url: "/Vendor/GetLoginVendorId",
	 //   dataType: "json",
        async:false,
	    success: function (responses) {
	        if (responses != "ss") {
             
	            $.ajax({
	                type: "GET",
	                url: "/api/vendors/DashBoard",
	                data: { vndr_Id: responses, Prop_Id: "", Days: 90 },
	                dataType: "json",
	                async: false,
	                success: function (response) {

	                    d_pie[0] = { label: "Booked", data: response.Table[2].orders }
	                    d_pie[1] = { label: "Aborted ", data: response.Table[0].orders }
	                    d_pie[2] = { label: "Hold ", data: response.Table[1].orders }
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

	

	$.plot("#chart_pie", d_pie, $.extend(true, {}, Plugins.getFlotDefaults(), {
		series: {
			pie: {
				show: true,
				radius: 1,
				label: {
					show: true
				}
			}
		},
		grid: {
			hoverable: true
		},
		tooltip: true,
		tooltipOpts: {
			content: '%p.0%, %s', // show percentages, rounding to 2 decimal places
			shifts: {
				x: 20,
				y: 0
			}
		}
	}));

});