\ USB driver

create usb:dev
  18 c,  \ bLength
  $01 c, \ USB_DEVICE_DESCRIPTOR_TYPE
  $00 c,
  $02 c, \ bcdUSB = 2.00
  $02 c, \ bDeviceClass: CDC
  $00 c, \ bDeviceSubClass
  $00 c, \ bDeviceProtocol
  $40 c, \ bMaxPacketSize0
  $83 c,
  $04 c, \ idVendor = 0x0483
  $40 c,
  $57 c, \ idProduct = 0x7540
  $00 c,
  $02 c, \ bcdDevice = 2.00
  1 c,     \ Index of string descriptor describing manufacturer
  2 c,     \ Index of string descriptor describing product
  3 c,     \ Index of string descriptor describing the device's serial number
  $01 c, \ bNumConfigurations
calign

create usb:conf  \ total length = 67 bytes
\ USB Configuration Descriptor
  9 c,   \ bLength: Configuration Descriptor size
  $02 c, \ USB_CONFIGURATION_DESCRIPTOR_TYPE
  67 c,  \ VIRTUAL_COM_PORT_SIZ_CONFIG_DESC
  0 c,
  2 c,   \ bNumInterfaces: 2 interface
  1 c,   \ bConfigurationValue
  0 c,   \ iConfiguration
  $C0 c, \ bmAttributes: self powered
  $32 c, \ MaxPower 0 mA
\ Interface Descriptor
  9 c,   \ bLength: Interface Descriptor size
  $04 c, \ USB_INTERFACE_DESCRIPTOR_TYPE
  $00 c, \ bInterfaceNumber: Number of Interface
  $00 c, \ bAlternateSetting: Alternate setting
  $01 c, \ bNumEndpoints: One endpoints used
  $02 c, \ bInterfaceClass: Communication Interface Class
  $02 c, \ bInterfaceSubClass: Abstract Control Model
  $01 c, \ bInterfaceProtocol: Common AT commands
  $00 c, \ iInterface:
\ Header Functional Descriptor
  5 c,   \ bLength: Endpoint Descriptor size
  $24 c, \ bDescriptorType: CS_INTERFACE
  $00 c, \ bDescriptorSubtype: Header Func Desc
  $10 c, \ bcdCDC: spec release number
  $01 c,
\ Call Management Functional Descriptor
  5 c,   \ bFunctionLength
  $24 c, \ bDescriptorType: CS_INTERFACE
  $01 c, \ bDescriptorSubtype: Call Management Func Desc
  $00 c, \ bmCapabilities: D0+D1
  $01 c, \ bDataInterface: 1
\ ACM Functional Descriptor
  4 c,   \ bFunctionLength
  $24 c, \ bDescriptorType: CS_INTERFACE
  $02 c, \ bDescriptorSubtype: Abstract Control Management desc
  $02 c, \ bmCapabilities
\ Union Functional Descriptor
  5 c,   \ bFunctionLength
  $24 c, \ bDescriptorType: CS_INTERFACE
  $06 c, \ bDescriptorSubtype: Union func desc
  $00 c, \ bMasterInterface: Communication class interface
  $01 c, \ bSlaveInterface0: Data Class Interface
\ Endpoint 2 Descriptor
  7 c,   \ bLength: Endpoint Descriptor size
  $05 c, \ USB_ENDPOINT_DESCRIPTOR_TYPE
  $82 c, \ bEndpointAddress: (IN2)
  $03 c, \ bmAttributes: Interrupt
  8 c,   \ VIRTUAL_COM_PORT_INT_SIZE
  0 c,
  $FF c, \ bInterval:
\ Data class interface descriptor
  9 c,   \ bLength: Endpoint Descriptor size
  $04 c, \ USB_INTERFACE_DESCRIPTOR_TYPE
  $01 c, \ bInterfaceNumber: Number of Interface
  $00 c, \ bAlternateSetting: Alternate setting
  $02 c, \ bNumEndpoints: Two endpoints used
  $0A c, \ bInterfaceClass: CDC
  $00 c, \ bInterfaceSubClass:
  $00 c, \ bInterfaceProtocol:
  $00 c, \ iInterface:
\ Endpoint 3 Descriptor
  7 c,   \ bLength: Endpoint Descriptor size
  $05 c, \ USB_ENDPOINT_DESCRIPTOR_TYPE
  $03 c, \ bEndpointAddress: (OUT3)
  $02 c, \ bmAttributes: Bulk
  64 c,  \ VIRTUAL_COM_PORT_DATA_SIZE
  0 c,
  $00 c, \ bInterval: ignore for Bulk transfer
\ Endpoint 1 Descriptor
  7 c,   \ bLength: Endpoint Descriptor size
  $05 c, \ USB_ENDPOINT_DESCRIPTOR_TYPE
  $81 c, \ bEndpointAddress: (IN1)
  $02 c, \ bmAttributes: Bulk
  64 c,  \ VIRTUAL_COM_PORT_DATA_SIZE
  0 c,
  $00 c, \ bInterval
calign

create usb:langid
  4 c, 3 c,  \ USB_STRING_DESCRIPTOR_TYPE,
  $0409 h, \ LangID = U.S. English

create usb:vendor
  40 c, 3 c,  \ USB_STRING_DESCRIPTOR_TYPE,
  char M h, char e h, char c h, char r h, char i h, char s h, char p h,
  bl     h, char ( h, char S h, char T h, char M h, char 3 h, char 2 h,
  char F h, char 1 h, char 0 h, char x h, char ) h,

create usb:product
  36 c, 3 c,  \ USB_STRING_DESCRIPTOR_TYPE,
  char F h, char o h, char r h, char t h, char h h, bl     h, char S h,
  char e h, char r h, char i h, char a h, char l h, bl     h, char P h,
  char o h, char r h, char t h,



