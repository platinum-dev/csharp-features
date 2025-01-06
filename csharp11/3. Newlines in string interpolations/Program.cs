var number1 = 1;
var number2 = 2;
var test = true;

_ = $"interpolated string with value {(test
    ? number1
    : number2
    )}, and some more text ...";