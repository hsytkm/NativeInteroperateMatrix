#pragma once

namespace NimaNativeCore {

    // 構造体の並びはC#と合わせているので変更しないこと！
    template <typename T>
    struct MatrixBase
    {
        void* pointer_;
        int rows_;          // _rows=Height
        int columns_;       // _columns=Width
        int bytesPerData_;
        int stride_;

        bool is_valid() const
        {
            if (pointer_ == nullptr) return false;
            if (columns_ <= 0 || rows_ <= 0) return false;
            if (bytesPerData_ <= 0) return false;
            if (stride_ < columns_ * bytesPerData_) return false;
            if (stride_ % bytesPerData_ != 0) return false;
            return true;    //valid
        }

        inline T* get_value_ptr_rc(int row, int col) const
        {
            auto ptr = (unsigned char*)pointer_ + (row * stride_);
            return (T*)ptr + col;
        }

        inline T& get_value_ref_rc(int row, int col) const
        {
            return *(get_value_ptr_rc(row, col));
        }

        inline T get_value_rc(int row, int col) const
        {
            return *(get_value_ptr_rc(row, col));
        }

        void fill_value(T value)
        {
            auto head = (unsigned char*)pointer_;
            int row_offset = rows_ * stride_;

            for (auto ptr_row = head; ptr_row < head + row_offset; ptr_row += stride_) {
                for (auto ptr = (T*)ptr_row; ptr < (T*)ptr_row + columns_; ptr++) {
                    *((T*)ptr) = value;
                }
            }
        }

        int copy_to(MatrixBase& dest_matrix) const
        {
            if (pointer_ == dest_matrix.pointer_) return -1;

            if (rows_ != dest_matrix.rows_ || columns_ != dest_matrix.columns_
                || bytesPerData_ != dest_matrix.bytesPerData_ || stride_ != dest_matrix.stride_)
            {
                return -2;
            }

            auto src = (const unsigned char*)pointer_;
            auto dest = (unsigned char*)dest_matrix.pointer_;
            int row_offset = rows_ * stride_;
            int col_offset = columns_ * bytesPerData_;
            for (size_t row = 0; row < row_offset; row += stride_) {
                for (size_t offset = row; offset < row + col_offset; offset += bytesPerData_) {
                    *((T*)(dest + offset)) = *((T*)(src + offset));
                }
            }
            return 0;
        }

#if true    //for test
        long long get_sum_int64() const
        {
            long long sum = 0;
            int row_offset = rows_ * stride_;
            auto head = (const unsigned char*)pointer_;

            for (auto ptr_row = head; ptr_row < head + row_offset; ptr_row += stride_) {
                for (auto ptr = (T*)ptr_row; ptr < (T*)ptr_row + columns_; ptr++) {
                    sum += *ptr;
                }
            }
            return sum;
        }

        double get_sum_double() const
        {
            double sum = .0;
            int row_offset = rows_ * stride_;
            auto head = (const unsigned char*)pointer_;

            for (auto ptr_row = head; ptr_row < head + row_offset; ptr_row += stride_) {
                for (auto ptr = (T*)ptr_row; ptr < (T*)ptr_row + columns_; ptr++) {
                    sum += *ptr;
                }
            }
            return sum;
        }
#endif

    };
}
