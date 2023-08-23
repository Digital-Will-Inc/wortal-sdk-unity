Module.allocString = function(str) {
    if (typeof str === "undefined" || str === null) {
        str = "";
    }
    const bufferSize = lengthBytesUTF8(str) + 1;
    const buffer = _malloc(bufferSize);
    stringToUTF8(str, buffer, bufferSize);
    return buffer;
}
