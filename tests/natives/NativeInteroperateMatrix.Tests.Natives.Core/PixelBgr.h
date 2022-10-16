#pragma once
#include <cstdint>

namespace NimaNativeCore {

    struct PixelBgr
    {
        uint8_t blue;
        uint8_t green;
        uint8_t red;

        PixelBgr() : blue{ 0 }, green{ 0 }, red{ 0 } { }

        explicit PixelBgr(uint8_t b, uint8_t g, uint8_t r)
            : blue{ b }, green{ g }, red{ r }
        { }
    };
}
