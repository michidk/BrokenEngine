#version 420

in vec4 f_color; // must match name in vertex shader
out vec4 f_fragColor; // first out variable is automatically written to the screen

void main()
{
    f_fragColor = f_color;
}