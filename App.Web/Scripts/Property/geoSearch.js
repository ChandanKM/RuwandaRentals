


$(function () {
    var lat = -33.8688,
        lng = 151.2195,
        latlng = new google.maps.LatLng(lat, lng),
        image = 'http://www.google.com/intl/en_us/mapfiles/ms/micons/blue-dot.png';

    var mapOptions = {
        center: new google.maps.LatLng(lat, lng),
        zoom: 13,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    },
        map = new google.maps.Map(document.getElementById('map_canvas'), mapOptions),
        marker = new google.maps.Marker({
            position: latlng,
            map: map,
            icon: image
        });

    var input = document.getElementById('searchTextField');
    var autocomplete = new google.maps.places.Autocomplete(input, {
        types: ["geocode"]
    });

    autocomplete.bindTo('bounds', map);
    var infowindow = new google.maps.InfoWindow();

    google.maps.event.addListener(autocomplete, 'place_changed', function () {
        infowindow.close();
        var place = autocomplete.getPlace();
        if (place.geometry.viewport) {
            map.fitBounds(place.geometry.viewport);
        } else {
            map.setCenter(place.geometry.location);
            map.setZoom(17);
        }

        moveMarker(place.name, place.geometry.location);
    });

    //$("input").focusin(function () {
    //    $(document).keypress(function (e) {
    //        if (e.which == 13) {
    //            infowindow.close();
    //            var firstResult = $(".pac-container .pac-item:first").text();

    //            var geocoder = new google.maps.Geocoder();
    //            geocoder.geocode({ "address": firstResult }, function (results, status) {
    //                if (status == google.maps.GeocoderStatus.OK) {
    //                    var lat = results[0].geometry.location.lat(),
    //                        lng = results[0].geometry.location.lng(),
    //                        placeName = results[0].address_components[0].long_name,
    //                        latlng = new google.maps.LatLng(lat, lng);

    //                    moveMarker(placeName, latlng);
    //                    $("input").val(firstResult);
    //                }
    //            });
    //        }
    //    });
    //});

    function moveMarker(placeName, latlng) {
        marker.setIcon(image);
        marker.setPosition(latlng);
        infowindow.setContent(placeName);
        infowindow.open(map, marker);
    }
});


