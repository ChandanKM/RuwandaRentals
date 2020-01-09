/*
 * charts/chart_donut.js
 *
 * Demo JavaScript used on charts-page for "Donut Chart".
 */

"use strict";

$(document).ready(function(){

	// Sample Data
	var d_donut = [];
	var series = 3;
    //for (var i = 0; i<series; i++) {
    //	d_pie[i] = { label: "Series "+(i+1), data: 33 }
    //}
	//d_donut[0] = { label: "Booked", data: 70 }
	//d_donut[1] = { label: "Cancelled ", data: 15 }
	//d_donut[2] = { label: "Hold ", data: 15 }
	$.ajax({
	    type: "GET",
	    url: "/api/Booking/DashBoard",
	    data: { vndr_Id: "", Prop_Id: "", Days: 90 },
	    dataType: "json",
	    async: false,
	    success: function (response) {
	        
	        d_donut[0] = { label: "Aborted", data: response.Table[0].orders }
	        d_donut[1] = { label: "Booked ", data: response.Table[2].orders }
	        d_donut[2] = { label: "Hold ", data: response.Table[1].orders }
	    },
	    error: function (jqxhr) {

	        // Failed(JSON.parse(jqxhr.responseText));
	    }
	});

	$.plot("#chart_donut", d_donut, $.extend(true, {}, Plugins.getFlotDefaults(), {
		series: {
			pie: {
				show: true,
				innerRadius: 0.5,
				radius: 1
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