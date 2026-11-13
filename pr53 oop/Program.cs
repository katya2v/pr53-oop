using System.Globalization;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

const string fileName = "numbers.txt";
string filePath = Path.Combine(AppContext.BaseDirectory, fileName);
Encoding encoding = Encoding.ASCII;

int firstNumber = 1;
int secondNumber = 27;
int thirdNumber = 314;
int replacementNumber = 2026;

// 1. Створити текстовий файл numbers.txt у каталозі програми й записати в
// нього ціле число у форматі з восьми цифр, доповнених зліва нулями,
// наприклад 00000001.
using (StreamWriter writer = new StreamWriter(filePath, false, encoding))
{
    writer.WriteLine(firstNumber.ToString("D8", CultureInfo.InvariantCulture));
}
Console.WriteLine($"Створено файл і записано число {firstNumber:D8}.");

// 2. Прочитати це число з файла, перетворити його на ціле число типу int і
// вивести на екран.
int numberFromFile;
using (StreamReader reader = new StreamReader(filePath, encoding))
{
    string? line = reader.ReadLine();
    numberFromFile = int.Parse(line!, CultureInfo.InvariantCulture);
}
Console.WriteLine($"Перше число як int: {numberFromFile}");

// 3. Дописати у кінець файла ще два рядки з іншими числами в такому ж
// форматі(кожне число в окремому рядку, числа мають відрізнятися від
// першого й одне від одного).
using (StreamWriter writer = new StreamWriter(filePath, true, encoding))
{
    writer.WriteLine(secondNumber.ToString("D8", CultureInfo.InvariantCulture));
    writer.WriteLine(thirdNumber.ToString("D8", CultureInfo.InvariantCulture));
}

// 4. Прочитати всі рядки файла й вивести їх на екран із номерами.
Console.WriteLine("Вміст файла з номерами рядків:");
PrintNumberedLines(filePath, encoding);

// Замінити друге число в файлі на нове значення в тому ж форматі
// безпосередньо у файлі, не переписуючи його весь вміст, прочитати файл
// ще раз і вивести новий вміст файла на екран.
long secondLineOffset =
    encoding.GetByteCount(firstNumber.ToString("D8", CultureInfo.InvariantCulture)) +
    encoding.GetByteCount(Environment.NewLine);

using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
using (StreamWriter writer = new StreamWriter(stream, encoding, 1024, leaveOpen: true))
{
    writer.Flush();
    writer.BaseStream.Seek(secondLineOffset, SeekOrigin.Begin);
    writer.Write(replacementNumber.ToString("D8", CultureInfo.InvariantCulture));
    writer.Flush();
}

Console.WriteLine($"Друге число замінено на {replacementNumber:D8}.");
Console.WriteLine("Новий вміст файла:");
PrintNumberedLines(filePath, encoding);

static void PrintNumberedLines(string path, Encoding fileEncoding)
{
    using (StreamReader reader = new StreamReader(path, fileEncoding))
    {
        int lineNumber = 1;
        string? line;

        while ((line = reader.ReadLine()) is not null)
        {
            Console.WriteLine($"{lineNumber}. {line}");
            lineNumber++;
        }
    }
}