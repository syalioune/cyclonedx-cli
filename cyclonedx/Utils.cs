// This file is part of CycloneDX CLI Tool
//
// Licensed under the Apache License, Version 2.0 (the “License”);
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an “AS IS” BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// SPDX-License-Identifier: Apache-2.0
// Copyright (c) Patrick Dwyer. All Rights Reserved.
using System;
using System.IO;
using CycloneDX.Models.v1_3;

namespace CycloneDX.CLI
{
    public static class CLIUtils
    {
        public static BomFormat DetectFileFormat(string filename)
        {
            var fileExtension = Path.GetExtension(filename);
            if (fileExtension == ".json")
            {
                return BomFormat.Json;
            }
            else if (fileExtension == ".xml")
            {
                return BomFormat.Xml;
            }
            else if (fileExtension == ".spdx")
            {
                return BomFormat.SpdxTag;
            }
            else if (fileExtension == ".csv")
            {
                return BomFormat.Csv;
            }
            else
            {
                return BomFormat.Unsupported;
            }
        }

        public static CycloneDX.Models.v1_3.Bom BomDeserializer(string bom, BomFormat format)
        {
            if (format == BomFormat.Json)
            {
                return Json.Deserializer.Deserialize(bom);
            }
            else if (format == BomFormat.Xml)
            {
                return Xml.Deserializer.Deserialize(bom);
            }
            else if (format == BomFormat.Csv)
            {
                return CsvSerializer.Deserialize(bom);
            }
            throw new UnsupportedFormatException("Unsupported BOM file format");
        }

        public static string BomSerializer(Bom bom, BomFormat format)
        {
            if (format == BomFormat.Json || format == BomFormat.Json_v1_3)
            {
                return Json.Serializer.Serialize(bom);
            }
            else if (format == BomFormat.Json_v1_2)
            {
                return Json.Serializer.Serialize(new CycloneDX.Models.v1_2.Bom(bom));
            }
            else if (format == BomFormat.Xml || format == BomFormat.Xml_v1_3)
            {
                return Xml.Serializer.Serialize(bom);
            }
            else if (format == BomFormat.Xml_v1_2)
            {
                return Xml.Serializer.Serialize(new CycloneDX.Models.v1_2.Bom(bom));
            }
            else if (format == BomFormat.Xml_v1_1)
            {
                var v1_2_bom = new CycloneDX.Models.v1_2.Bom(bom);
                return Xml.Serializer.Serialize(new CycloneDX.Models.v1_1.Bom(v1_2_bom));
            }
            else if (format == BomFormat.Xml_v1_0)
            {
                var v1_2_bom = new CycloneDX.Models.v1_2.Bom(bom);
                var v1_1_bom = new CycloneDX.Models.v1_1.Bom(v1_2_bom);
                return Xml.Serializer.Serialize(new CycloneDX.Models.v1_0.Bom(v1_1_bom));
            }
            else if (format == BomFormat.SpdxTag || format == BomFormat.SpdxTag_v2_2)
            {
                return SpdxTagSerializer.Serialize(bom, SpdxVersion.v2_2);
            }
            else if (format == BomFormat.SpdxTag_v2_1)
            {
                return SpdxTagSerializer.Serialize(bom, SpdxVersion.v2_1);
            }
            else if (format == BomFormat.Csv)
            {
                return CsvSerializer.Serialize(bom);
            }
            throw new UnsupportedFormatException("Unsupported BOM file format");
        }

        public static string NullIfWhiteSpace(this string str) {
            return String.IsNullOrWhiteSpace(str) ? null : str;
        }
    }
}