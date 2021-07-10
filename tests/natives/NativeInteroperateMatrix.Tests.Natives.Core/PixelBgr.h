#pragma once

namespace NimaNativeCore {

    struct PixelBgr
    {
        unsigned char blue;
        unsigned char green;
        unsigned char red;

        PixelBgr() : blue{ 0 }, green{ 0 }, red{ 0 } { }

        explicit PixelBgr(unsigned char b, unsigned char g, unsigned char r)
            : blue{ b }, green{ g }, red{ r }
        { }
    };
}
