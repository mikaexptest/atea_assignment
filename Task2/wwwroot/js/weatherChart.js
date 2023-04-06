$(document).ready(function () {
    update();
});

const temperature = 1;
const wind = 2;
var nReloads = 0;
var emptyWeatherAPIInfo = { id: 0, cityId: 0, city: '', state: '', cityState: '', temperature: 0, windSpeed: 0, utcTimestamp: '' };
var barColorsArray = ["#ea5545", "#fd7f6f", "#7eb0d5", "#b2e061", "#bd7ebe", "#ffb55a", "#ffee65", "#beb9db", "#fdcce5", "#8bd3c7"]

var morrisBarTemperature = Morris.Bar({
    element: 'ateatemperaturechart',
    data: [
        emptyWeatherAPIInfo
    ],
    xkey: 'cityState',
    ykeys: ['temperature'],
    labels: ['Temperature'],
    hideHover: true,
    barColors: function (row, series, type) {
        return barColorsArray[row.x];
    },
}).on('click', function (a, row) {
    $("#ateatemperaturelinelabel").text(row.cityState + " => ");
    getWeatherTrend(row, temperature);
});

var morrisLineTemperature = Morris.Line({
    element: 'ateatemperatureline',
    data: [
        emptyWeatherAPIInfo
    ],
    xkey: 'utcTimestamp',
    ykeys: ['temperature'],
    labels: ['Temperature'],
    pointFillColors: ['#ffffff'],
    pointStrokeColors: ['black'],
    hideHover: true
});

var morrisBarWind = Morris.Bar({
    element: 'ateawindchart',
    data: [
        emptyWeatherAPIInfo
    ],
    xkey: 'cityState',
    ykeys: ['windSpeed'],
    labels: ['Wind Speed'],
    barColors: ['red'],
    hideHover: true
}).on('click', function (a, row) {
    $("#ateawindlinelabel").text(row.cityState + " => ");
    getWeatherTrend(row, wind);
});

var morrisLineWind = Morris.Line({
    element: 'ateawindline',
    data: [
        emptyWeatherAPIInfo
    ],
    xkey: 'utcTimestamp',
    ykeys: ['windSpeed'],
    labels: ['Wind Speed'],
    pointFillColors: ['#ffffff'],
    pointStrokeColors: ['black'],
    hideHover: true
});

function update() {
    nReloads++;

    $.ajax({
        method: "GET",
        cache: false,
        url: "/Charts/GetNewWeatherData",
        contentType: "application/json",
        data: {},
        success: function (data) {
            if (data != null) {
                morrisBarTemperature.setData(data.sort(function (a, b) {
                    return (a.temperature - b.temperature);
                }));

                morrisBarWind.setData(data.sort(function (a, b) {
                    return - (a.windSpeed - b.windSpeed);
                }));
            }
            else {
                // render empty
            }
        },
        error: function () {
            // render empty
        }
    });

    $('#reloadStatus').text(nReloads + ' reloads. Timestamp: ' + new Date().toISOString());
}

function getWeatherTrend(rowData, type) {
    $.ajax({
        method: "GET",
        cache: false,
        url: "/Charts/GetWeatherTrendByCity",
        contentType: "application/json",
        data: rowData,
        success: function (data) {
            if (data != null) {
                if (type == temperature) {
                    morrisLineTemperature.setData(data);
                } else if (type == wind) {
                    morrisLineWind.setData(data);
                }
            }
            else {
                // render empty
            }
        },
        error: function () {
            // render empty
        }
    });
}

setInterval(update, 60 * 1000);