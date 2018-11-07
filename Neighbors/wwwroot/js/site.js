function showCurrencies() {
	let url = "http://openexchangerates.org/api/latest.json?app_id=d04a1cbf0f14493c973fc7087c0629fb"
	$.get(url).then(function (response) {
		let price_input = $("#price_input").val()
		if (price_input !== '') {
			let usd_ils_rate = response.rates['ILS']
			let amount_in_usd = parseFloat(price_input) / parseFloat(usd_ils_rate)
			let amount_in_usd_str = amount_in_usd.toString()
			let dot_index = amount_in_usd_str.indexOf(".")
			if (dot_index > 0) {
				amount_in_usd_str = amount_in_usd_str.substring(0, dot_index + 3)
			}
			console.log(amount_in_usd_str)
			$('#usd_value').html('(' + amount_in_usd_str + ' $)')
		}

	});
}
