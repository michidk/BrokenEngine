#version 420

uniform mat4 modelViewProjMatrix;

// attributes of our vertex
in vec3 vPosition;
in vec4 vColor;
out vec4 fColor; // must match name in fragment shader

void main()
{
    // gl_Position is a special variable of OpenGL that must be set
    gl_Position = modelViewProjMatrix * vec4(vPosition, 1.0);
    fColor = vColor;
}