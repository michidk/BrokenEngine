#version 420

uniform mat4 u_modelViewProjMatrix;

// attributes of our vertex
in vec3 v_position;
in vec4 v_color;
out vec4 f_color; // must match name in fragment shader

void main()
{
    // gl_Position is a special variable of OpenGL that must be set
    gl_Position = u_modelViewProjMatrix * vec4(v_position, 1.0);
    f_color = v_color;
}