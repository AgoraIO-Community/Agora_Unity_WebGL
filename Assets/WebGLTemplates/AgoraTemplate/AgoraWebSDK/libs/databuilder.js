var DataBuilder = function () {
  var self = {
    data: Array(),
  };

  self.add = function (key, value) {
    self.data.push(key + ";" + value);
  };

  self.build = function () {
    var i = 0;
    var pre = "";
    var str = "";
    while (i < self.data.length) {
      str += pre + self.data[i];
      pre = "|";
      i++;
    }
    self.data = Array();
    return str;
  };

  return self;
};
