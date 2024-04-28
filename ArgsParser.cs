using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;

namespace HasherLib 
{
    public class Options
    {
        private string[] _files = Array.Empty<string>();

        [Option('f', "files", Required = true, HelpText = "File paths")]
        public IEnumerable<string> Files
        {
            get => _files;
            set => _files = value.ToArray();
        }

        public string[] FilesArray => _files;

        [Option('m', "mode", Required = true, HelpText = "Type of hash (CRC32 or SUM32)")]
        public string HashType { get; set; }
    }

    public class ArgsParser
    {
        public List<string> FilePaths { get; }
        public HasherType Mode { get; private set; }

        public ArgsParser(string[] args)
        {
            FilePaths = new List<string>();
            Parse(args);
        }

        private void Parse(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments<Options>(args)
                    .WithParsed(options =>
                    {
                        if (options.Files != null)
                        {
                            foreach (string file in options.Files)
                            {
                                if (File.Exists(file))
                                {
                                    FilePaths.Add(file);
                                }
                                else
                                {
                                    Console.WriteLine($"File {file} does not exist");
                                }
                            }
                        }

                        if (options.HashType != null)
                        {
                            Mode = options.HashType.ToLower() switch
                            {
                                "crc32" => HasherType.CRC32,
                                "sum32" => HasherType.SUM32,
                                _ => throw new ArgumentException("Incorrect mode. Use: CRC32 or SUM32."),
                            };
                        }
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}