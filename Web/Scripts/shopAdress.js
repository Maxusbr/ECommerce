ymaps.ready(init);
function init() {
    var myMap;
    //myPlacemark,
    // Создадим экземпляр элемента управления «поиск по карте»
    // с установленной опцией провайдера данных для поиска по организациям.
    var searchControl = new ymaps.control.SearchControl({
        options: {
            provider: 'yandex#search'
        }
    });
    function correctAdress(firstGeoObject) {
        var _city = firstGeoObject.properties.get('metaDataProperty.GeocoderMetaData.AddressDetails.Country.AdministrativeArea.Locality.LocalityName');
        var _street = firstGeoObject.properties.get('metaDataProperty.GeocoderMetaData.AddressDetails.Country.AdministrativeArea.Locality.Thoroughfare.ThoroughfareName');
        var _house = firstGeoObject.properties.get('metaDataProperty.GeocoderMetaData.AddressDetails.Country.AdministrativeArea.Locality.Thoroughfare.Premise.PremiseNumber');
        $('.adressSity').val(_city);
        $('.adressStreet').val(_street);
        $('.adressHouse').val(_house);
    }
    function foundCoord(adress) {
        ymaps.geocode(adress, { results: 1 }).then(function (res) {
            var firstGeoObject = res.geoObjects.get(0),
                coords = firstGeoObject.geometry.getCoordinates(),
                bounds = firstGeoObject.properties.get('boundedBy');
            myMap.geoObjects.removeAll();
            myMap.geoObjects.add(firstGeoObject);
            correctAdress(firstGeoObject);
            myMap.setCenter(coords, 15);
        });
    }

    function createMap() {
        myMap = new ymaps.Map('map', {
            center: [50.4481, 30.5254],
            zoom: 11,
            type: 'yandex#map',
            controls: []
        });
        myMap.events.add('click', function (e) {
            var coords = e.get('coords');
            getAddress(coords);
        });
        //myMap.controls.add(searchControl);
    }

    function getStringAdress() {
        var res = $(".adressSity")[0].value;
        if ($(".adressStreet")[0].value) res += ", " + $(".adressStreet")[0].value;
        if ($(".adressHouse")[0].value) res += ", " + $(".adressHouse")[0].value;
        return res;
    }

    function changeAdress() {
        //if (!myMap) createMap();
        //searchControl.search(res);
        foundCoord(getStringAdress());
    }
    // Определяем адрес по координатам (обратное геокодирование)
    function getAddress(coords) {
        //myPlacemark.properties.set('iconContent', 'поиск...');
        ymaps.geocode(coords).then(function (res) {
            var firstGeoObject = res.geoObjects.get(0);
            if (firstGeoObject === null) {
                var _city = ''; // если не найден - возвращаем пустую строку
            } else {
                correctAdress(firstGeoObject);
                changeAdress();
            }
        });
    }

    $('#hideMap').bind({
        click: function () {
            if (myMap) {
                myMap.destroy();
                myMap = null;
            }
            $("#textAdress").show();
            $("#createAdress").hide();
            if ($("#FullAdress") != 'undefined') $("#FullAdress")[0].innerHTML = getStringAdress();
        }
    });

    $('#showMap').bind({
        click: function () {
            if (!myMap) {
                createMap();
                if ($(".adressSity")[0]) $('.adressSity').val("Киів");
                changeAdress();
            }
            $("#textAdress").hide();
            $("#createAdress").show();
        }
    });

    $(document).ready(function () {
        if ($("#showMap")[0] === undefined) return;
        if ($('#showMap')[0].innerHTML == "showMap") {
            //createMap();
            changeAdress();
        }
    });
    
    
    $(".adressInput").change(changeAdress);
}







