using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace NFTMetadataCreator_CLI
{
    internal static class Program
    {
        private static NftMetadataDto _nftDto = new();
        
        private static string _outputLocation = "";
        private static string _nftName = "";
        private static string _nftDescription = "";
        private static string _nftImage = "";

        private static bool _skipConfirms;
        private static bool _noOutput;
        private static bool _needAttributes = true;
        private static bool _printOutput = false;

        private static List<AttributeDto> _attributeList = new();
        
        
        static void Main(string[] args)
        {
            if (args is not null)
            {
                for (var i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "--output":
                        case "-o":
                            if (args.Length >= i + 2) //+2 because 0-based index but 1-based length
                            {
                                _outputLocation = args[i + 1];
                                i++; // Skip over the next argument as it was the file location
                                break;
                            }
                            else
                                Output($"{args[i]} ignored: No valid output file was defined");
                            break;
                        
                        case "--name":
                        case "-n":
                            if (args.Length >= i + 2)
                            {
                                _nftName = args[i + 1];
                                i++;
                                break;
                            }
                            else
                                Output($"{args[i]} ignored: Missing argument after. Please correct and try again.");
                            Environment.Exit(0);
                            break;       
                        
                        case "--description":
                        case "-d":
                            if (args.Length >= i + 2)
                            {
                                _nftDescription = args[i + 1];
                                i++;
                                break;
                            }
                            else
                                Output($"{args[i]} ignored: Missing argument after. Please correct and try again.");
                            Environment.Exit(0);
                            break;     
                        
                        case "--imageUri":
                        case "-i":
                            if (args.Length >= i + 2)
                            {
                                _nftImage = args[i + 1];
                                i++;
                                break;
                            }
                            else
                                Output($"{args[i]} ignored: Missing argument after. Please correct and try again.");
                            Environment.Exit(0);
                            break;
                        
                        case "--attributes":
                        case "-a":
                            if (args.Length >= i + 2)
                            {
                                var attributeArr = args[i + 1].Split('\\');
                                try
                                {
                                    for (int x = 0; x < attributeArr.Length; x += 2)
                                    {
                                        _attributeList.Add(new AttributeDto(attributeArr[x], attributeArr[x + 1]));
                                    }
                                }
                                catch
                                {
                                    Output("Failed to read the attributes, check your syntax. Exiting...");
                                    Environment.Exit(0);
                                }
                                _needAttributes = false;
                                i++;
                                break;
                            }
                            else
                                Output($"{args[i]} ignored: Missing argument after. Please correct and try again.");
                            Environment.Exit(0);
                            break;
                        
                        case "--skip-confirms":
                        case "-s":
                            _skipConfirms = true;
                            break;
                        
                        case "--no-ouput":
                        case "-no":
                            _noOutput = true;
                            break;
                        
                        case "--print-result":
                        case "-p":
                            _printOutput = true;
                            _outputLocation = "N/A";
                            break;

                        default:
                            Output(args[i] + " was not found as a valid argument. Please fix your arguments and try again. Refer to the documentation for more details.");
                            Environment.Exit(0);
                            break;
                    }
                }
            }
            
            Output("Welcome to the NFT Metadata creator");

            // Defaults
            if (_outputLocation == "")
                _outputLocation = "metadata.json";

            Output("Your metadata will be saved to " + _outputLocation, "");
            if (!_skipConfirms)
            {
                Output("Press any key to continue...", "");
                Console.ReadKey();
            }
            Output();
            
            Console.Clear();

            if (_nftName == "")
            {
                Console.Clear();
                Output("Please enter a name for the NFT: ");
                _nftName = Console.ReadLine();
            }

            if (_nftDescription == "")
            {
                Console.Clear();
                Output("Please enter a description for the NFT: ");
                _nftDescription = Console.ReadLine();
            }

            if (_nftImage == "")
            {
                Console.Clear();
                Output("Please enter the URI of the image (example https://ethanrushbrook.com/api/item/1.png): ");
                _nftImage = Console.ReadLine();
            }

            if (_needAttributes)
                while (true)
                {
                    Console.Clear();
                    Output("Please enter all of your desired attributes. To finish, enter nothing and press your enter key.");
                    Output();
                    
                    Console.Write("Trait Name: ");
                    var traitName = Console.ReadLine();
                    if (traitName is null or "") // Just in case the console stream dies or to exit when done
                        break;
                    
                    Console.Write("Trait Value: ");
                    var traitValue = Console.ReadLine();
                    
                    _attributeList.Add(new AttributeDto(traitName, traitValue));
                }
            
            //Time to populate the DTO
            _nftDto = new NftMetadataDto
            {
                name = _nftName,
                description = _nftDescription,
                image = _nftImage,
                attributes = _attributeList
            };

            var jsonOutput = JsonConvert.SerializeObject(_nftDto, Formatting.Indented);

            Console.Clear();
            if (_printOutput)
                Console.WriteLine(jsonOutput);
            else
                try
                {
                    File.WriteAllBytes(_outputLocation, Encoding.UTF8.GetBytes(jsonOutput));
                }
                catch (Exception ex)
                {
                    Output("Failed to save the file. Check your output file and that you have writing permissions to that location. Exiting...");
                    Environment.Exit(0);
                }
            
            Output($"Successfully wrote the output to {_outputLocation}. Exiting...");
        }
        
        private static void Output(string text = "", string ending = "\n")
        {
            if (_noOutput)
                return;
            
            Console.Write(text + ending);
        }
    }

    class NftMetadataDto
    {
        public string name { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public List<AttributeDto> attributes { get; set; }
    }

    class AttributeDto //We need this to force the JSON serializer to serialize in the way we want
    {
        public string trait_type { get; set; }
        public string value { get; set; }

        public AttributeDto(string traitType, string value)
        {
            this.trait_type = traitType;
            this.value = value;
        }
    }
}
