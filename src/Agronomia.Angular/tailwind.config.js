/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  darkMode: "class",
  theme: {
    extend: {
      colors: {
        primary: "#50da0b",
        "primary-dark": "#42b609",
        "primary-hover": "#44b909",
        "background-light": "#f6f8f5",
        "background-dark": "#162210",
        "surface-light": "#ffffff",
        "surface-dark": "#1c2b18",
        "text-main": "#131811",
        "text-main-light": "#121c0d",
        "text-main-dark": "#f0fdf4",
        "text-muted": "#6e8a60",
        "text-secondary-light": "#659c49",
        "text-secondary-dark": "#a3d984",
        "border-color": "#dfe6db",
        "border-light": "#ebf4e7",
        "border-dark": "#2a3c22",
      },
      fontFamily: {
        display: ["Manrope", "sans-serif"],
      },
      borderRadius: {
        DEFAULT: "0.25rem",
        lg: "0.5rem",
        xl: "0.75rem",
        full: "9999px",
      },
    },
  },
  plugins: [],
};
