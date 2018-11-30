window.copyToClipboard = function (text) {
  var host = document.createElement("textarea");
  host.value = text;
  document.body.appendChild(host);
  host.select();
  document.execCommand("copy");
  document.body.removeChild(host);

  console.log("Copied permalink to clipboard: " + text);
  return true;
};

window.openInBlankTab = function (url) {
  window.open(url, "_blank");
  return true;
};
