window.downloadFromBase64 = function (base64, fileName) {
  var byteCharacters = atob(base64)
  var byteNumbers = new Array(byteCharacters.length)
  for (var i = 0; i < byteCharacters.length; i++) {
    byteNumbers[i] = byteCharacters.charCodeAt(i)
  }
  var byteArray = new Uint8Array(byteNumbers)
  var blob = new Blob([byteArray], { type: "application/octet-stream" })
  var url = URL.createObjectURL(blob)
  var link = document.createElement("a")
  link.href = url
  link.download = fileName
  link.click()
  URL.revokeObjectURL(url)
  link.remove()
}
