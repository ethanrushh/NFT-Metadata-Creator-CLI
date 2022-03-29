# NFT-Metadata-Creator-CLI
A simple but powerful CLI-based JSON metadata creator for NFTs. This has been designed to reflect the standard OpenSea uses for their NFT metadata and will be compatible with their platform.

## Arguments
**Important**: None of these options must be set, however they must all be correctly configured for a no-input based run. If you want to type in your inputs, you do NOT have to set these before running the program.
### Output
`-o <path>` or `--ouput <path>`

Manually specifies the file output. If not set, the default is metadata.json (in the current working directory). Must be a valid file location and the extension should be .json (however this is not a hard requirement)

### Name
`-n <name>` or `--name <name>`

Specifies the name of the NFT

### Description
`-d <description>` or `--description <description>`

Specifies the description of the NFT

### Image URI
`-i <URI>` or `--image-uri <URI>`

Specifies the URI (or URL) used for the image

### Attributes
`-a <attributes>` or `--attributes <attributes>`

Specifies the attributes to be used for the metadata. Format is name\value. You can join multiple attributes into the same NFT using another **\\**. If there are spaces in the attributes' names or values, you must use a **"** at the start and end of the argument to avoid starting a new argument.

Example with 2 attributes:
`"<attribute name>\<attribute value>\<second attribute name>\<second attribute value>"`

### Skip Confirmations
`-s` or `--skip-confirms`

Skips all confirmations. This is not recommended when using the program standalone (i.e. running it in a terminal with missing optional arguments) but is extremely useful when calling from another program. If all options are set, this will ensure output is processed without interuption if all arguments are correctly set. 

### No Console Output
`--no` or `--no-output`

Prevents the console from outputting any information regarding inputs or confirmations. When used with the `-p` option, the JSON output is still printed but nothing else is. Generally used with `-p`.

### Print JSON Result
`-p` or `--print-result`

Prints the resulting JSON file to the terminal. This is extremely useful if called from another program as the output can be directly read from the IO stream and processed. Recommended to be used with the `-s` option.

## Full Example:
The following output is achieved:
```json
{
  "name": "Test NFT",
  "description": "An imaginary test NFT",
  "image": "image.url",
  "attributes": [
    {
      "trait_type": "creator",
      "value": "Ethan Rushbrook"
    },
    {
      "trait_type": "Reading this on",
      "value": "Github"
    }
  ]
}
```

using the arguments `-o output.json -a "creator\Ethan Rushbrook\Reading this on\Github" -n "Test NFT" -d "An imaginary test NFT" -i image.url -s -no`

## Donations
Think my work is cool? Chip me some coin on any EVM-compatible network at 0x2F50B7A73D7065a672bB0eb97fB4e3DAb391CC23 and make my day.
