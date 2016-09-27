//# Name Vertex Color
//# Author Michael Lohr
//# Description Just renders the vertex color, nothing else.

//# Type VERTEX
#version 420

uniform mat4 u_modelViewProjMatrix;

// vertex attributes
in vec3 v_position;
in vec4 v_color;

// output
out vec4 f_color;

void main()
{
    // gl_Position is a special variable of OpenGL that has to be set
    gl_Position = u_modelViewProjMatrix * vec4(v_position, 1.0);
    f_color = v_color;
}

//# Type FRAGMENT
#version 420

in vec4 f_color; // must match name in vertex shader
out vec4 f_fragColor; // first out variable is automatically written to the screen

void main()
{
    f_fragColor = f_color;
}