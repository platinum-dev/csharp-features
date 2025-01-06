#region ref

Data data1 = new(1);
Ref(ref data1);
Console.WriteLine(data1.Value);

void Ref(ref Data data)
{
    data.Value += 1;
}

#endregion

#region out

Data data2;
Out(2, out data2);
Console.WriteLine(data2.Value);

void Out(int value, out Data data)
{
    data = new(value);
    data.Value += 1;
}

#endregion

#region in

Data data3 = new(3);
In(data3);
In(in data3);
In(ref data3);
In(data3 = new Data());

void In(in Data data)
{
    // data.Value = 1;
    // data.Value += 1;
}

#endregion

#region ref readonly

Data data4 = new(4);
RefReadonly(data4);
RefReadonly(in data4);
RefReadonly(ref data4);
RefReadonly(data4 = new Data());

void RefReadonly(ref readonly Data data)
{
    // data.Value = 1;
    // data.Value += 1;
}

#endregion

// // rvalue and lvalue
// int x;
// // ...
// x = 1;
// // 1 = x;
// Console.WriteLine(x);


struct Data(int value)
{
    public int Value { get; set; } = value;
}