#version 420

in vec4 fColor; // must match name in vertex shader
out vec4 fragColor; // first out variable is automatically written to the screen

void main()
{
    fragColor = fColor;
}