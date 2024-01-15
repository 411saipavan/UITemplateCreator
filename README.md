# UITemplateCreator

This project creates 'MyTools' in Menubar and theres a 'Create Template' option under that. When that is clicked, a custom unity editor window opens.

This window can be used to create new UI templates which can be used to create UI hierarchies easily.

## How the window works:
There's an option to select the type of UI element(i.e, text, input field, button, etc). After that name,text,position,rotation,scale fields can be given as input from the corresponding input fields.

### How the text field works for each element type:
Text: The text is displayed in the text element
Input Field: The text is shown as the placeholder
Button: The text is shown as the Button text

When 'Add Element' button is clicked, it adds the element to the draft template. That draft template can be seen in the 'Template Preview' below it. In this Template Preview, we can edit the properties of the added elements, remove the elements or add child elements too. There's also an json preview which can be used for checking the hierarchy.

### How to add the child elements:
When 'Add Child Template' button is clicked under an element, the file explorer opens. From that we can select an existing template and all the elements in that template will be added as children to this element with the hierarchy preserved.

When the draft template is complete, we can continue with creating a json file to store this template. For that, first we need to create a new folder under 'Assets' in the project and name it as 'UI Templates'(Ignore if this folder already exists).
Then we can give the name of the file in the 'File Name' input field, and once some name is given in this field, 'Create Template' Button appears. Once this button is clicked, a new file with the given file name will be created.

### How to load an existing template:
For this, there's a 'Select File' button. Once this button is clicked, the File Explorer opens and an existing template can be selected. Once a file is selected, the template is loaded and that can be seen in the 'Template Preview'.

### How to instantiate a template:
Once the 'Instantiate Template' button is clicked, a canvas will be created and then all the elements with the hierarchy in the 'Template Preview' will be instantiated under the canvas. After this, for each element in the 'Template Preview', a new 'Update this element' button will be created. If any properties are changed and this button is clicked, that element will be updated with new properties.

## Next Steps:
1. More element types can be added.
2. More properties can be added while adding elements and the input fields can be changed according to the UIElement type.
3. Can add an option to store the prefabs also, which can make this tool much more useful.
